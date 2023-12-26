This is the backend of a full stack application for the management of a library. 
The first login is made by the admin whose password is defined in the appsettings file. 
The admin enters parameters such as subscription types and book categories and registers 
all other users.
In the app you can register book titles, members and their subscriptions and afterwards
you can make the book borrowing transactions. The borrowing service checks if 
a member has an active subscriptions, if he has not exceeded the borrowing
allowance specified by his subscription and if the requested book is available
at the moment.
A borroing is valid from the current day till the next 7 days after which it is
considered as expired. Expired borrowings can be checked by the app and the
necessary contact details retrieved.
Since for subscriptions and borroings the startign date is set automatically
as the current date, the app can be tested for its further fucntionality by
entering subscription and borrowing data in the db (whose script is also provided)
manually so that you can check that the borrowing and subscription history 
views work.
The front end is uploaded in another repository.
