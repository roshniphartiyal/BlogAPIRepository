create database BlogDatabase
go
use BLogDatabase;
go
create table UserTable (UserId int primary key identity, FirstName nvarchar(100) not null,
LastName nvarchar(100), Status int not null, AddedOn Datetime,
Password nvarchar(100) not null ,LoginName nvarchar(100) not null)
go
insert into UserTable values ('adminfirstname','adminlastname',1,GETDATE(),'adminlogin','adminpass')
go
create table BlogTable( BlogId int primary key identity,BlogTitle nvarchar(500),
userId int,BlogStatus int, Content nvarchar(max), LastModified Datetime )
go
create table ErrorLog (errorid int primary key identity, ErrorMessage nvarchar(max),
Severity nvarchar(10) ) 

