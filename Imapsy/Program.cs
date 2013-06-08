using ActiveUp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imapsy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("Welcome to Imapsy!");

                Console.WriteLine();
                var targetItems = GetItemsOnTarget(options);

                Console.WriteLine();
                CopyItemsFromSourceToTarget(options, targetItems);

                Console.WriteLine();
                Console.WriteLine("Everything done. Press < Enter > to quit.");
            }
        }

        private static IEnumerable<Item> GetItemsOnTarget(Options options)
        {
            var targetItems = new List<Item>();
            if (options.IgnoreExistingItems)
            {
                Console.WriteLine("Skipping the duplicate check ...");
                return targetItems;
            }

            Console.WriteLine("Downloading items from the target, and checking for duplicates ...");
            Connect(options.TargetHost, options.TargetPort, options.TargetUsername, options.TargetPassword, options.TargetFolder, options.TargetUseSsl, (server, mailbox) =>
                {
                    var count = mailbox.MessageCount;
                    for (var i = 1; i <= count; i++)
                    {
                        try
                        {
                            var rawHeader = mailbox.Fetch.Header(i);
                            var parsedHeader = Parser.ParseHeader(rawHeader);

                            Console.WriteLine("\t Downloaded {0}/{1}: '[{2}] {3}'", i, count, parsedHeader.Date, parsedHeader.Subject);
                            if (targetItems.Any(x => x.Date == parsedHeader.Date && x.Subject == parsedHeader.Subject))
                            {
                                mailbox.DeleteMessage(i, true);
                                Console.WriteLine("\t\t Deleted.");
                                i--;
                                count--;
                            }
                            else
                                targetItems.Add(new Item { Date = parsedHeader.Date, Subject = parsedHeader.Subject });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\t\t Error: " + ex.Message);
                        }
                    }
                });

            return targetItems;
        }

        private static void CopyItemsFromSourceToTarget(Options options, IEnumerable<Item> targetItems)
        {
            Console.WriteLine("Copying items from the source to the target ...");
            Connect(options.TargetHost, options.TargetPort, options.TargetUsername, options.TargetPassword, options.TargetFolder, options.TargetUseSsl,
                    (targetServer, targetMailbox) =>
                    {
                        Connect(options.SourceHost, options.SourcePort, options.SourceUsername, options.SourcePassword, options.SourceFolder, options.SourceUseSsl,
                                (sourceServer, sourceMailbox) =>
                                {
                                    var count = sourceMailbox.MessageCount;
                                    for (var i = options.StartCopyingAt; i <= count; i++)
                                    {
                                        var rawHeader = sourceMailbox.Fetch.Header(i);
                                        var parsedHeader = Parser.ParseHeader(rawHeader);

                                        Console.WriteLine("\t Downloaded {0}/{1}: '[{2}] {3}'", i, count, parsedHeader.Date, parsedHeader.Subject);

                                        if (targetItems.Any(x => x.Subject == parsedHeader.Subject && x.Date == parsedHeader.Date))
                                            Console.WriteLine("\t\t Skipped.");
                                        else
                                        {
                                            try
                                            {
                                                var rawMessage = sourceMailbox.Fetch.Message(i);

                                                var flags = new FlagCollection { new Flag("\\Seen") };
                                                targetMailbox.Append(rawMessage, flags, parsedHeader.Date.ToUniversalTime());
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("\t\t Error: " + ex.Message);
                                            }
                                        }
                                    }
                                });
                    });
        }

        private static void Connect(string host, int port, string username, string password, string folder, bool useSsl, Action<Imap4Client, Mailbox> whatToDo)
        {
            using (var server = new Imap4Client())
            {
                var mailbox = Connect(server, host, port, username, password, folder, useSsl);

                try
                {
                    whatToDo.Invoke(server, mailbox);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                server.Close();
            }
        }

        private static Mailbox Connect(Imap4Client server, string host, int port, string username, string password, string folder, bool useSsl)
        {
            if (useSsl)
                server.ConnectSsl(host, port);
            else
                server.Connect(host, port);
            server.Login(username, password);

            return server.AllMailboxes.Cast<Mailbox>().First(x => x.Name == folder);
        }

    }
}
