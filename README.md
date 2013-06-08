= Imapsy =

Imapsy is a little C# console application that helped me to copy my emails from Gmail to Office365.

I wrote Imapsy because Outlook and Thunderbird couldn't handle my 100.000+ e-mails, and because I couldn't get IMAPsize to work with my Office365 account.

Imapsy does two things:
1. Download all the headers from the target IMAP folder, and delete all duplicates from there.
2. Download every email from your source IMAP folder, and append them to your target IMAP folder as long as it doesn't exist there yet (this comes in handy if you want to run Imapsy more than once).

Imapsy will not:
- Work on your entire IMAP mailbox at once. You'll have to run it for each IMAP folder separately.
- Delete any email from your source IMAP folder.
- Blow your mind. It is just a few quick lines of code, neither pretty nor performant. I think most of the effort went into this Readme ;)

== Usage ==
    Imapsy.exe --sourceHost=imap.googlemail.com --sourceUsername=YOUR@SERVER.COM --sourcePassword=PASS "--sourceFolder=[Gmail]/All Mail" [--sourcePort=993] [--sourceUseSsl] --targetHost=outlook.office365.com --targetUsername=YOUR@SERVER.COM --targetPassword=PASS "--targetFolder=Deleted Items"  [--targetPort=993] [--targetUseSsl] [--ignoreExistingItems] [--startCopyingAt=1]
	
	Imapsy.exe --help
	
== Dependencies ==
Imapsy uses ActiveUp.MailSystem for all the IMAP communication (see http://mailsystem.codeplex.com/). Unfortunately it had a few bugs so I ended up including the source of the ActiveUp.Net.Imap4 library into my solution to fix these bugs. Please note that ActiveUp.MailSystem is distributed under the LGPL license (see license information in the /lib directory).

In hindsight I would not use ActiveUp.MailSystem again, because at the time of this writing it had _a lot_ of bugs and 90% of my time went into trying to work around them :(
	
== License ==
Do what ever you want to do with these few lines of code. Pull requests are always welcome of course.