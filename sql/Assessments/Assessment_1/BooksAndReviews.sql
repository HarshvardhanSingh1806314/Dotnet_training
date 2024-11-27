use Assessment

-- creating books table
create table Books(
	id int primary key,
	title varchar(30),
	author varchar(20),
	isbn int,
	publishedDate datetime
)

-- creating reviews table
create table Reviews(
	id int primary key,
	bookId int references Books(id),
	reviewerName varchar(20),
	content varchar(30),
	rating int,
	publishedDate datetime
)

-- inserting values into books
insert into Books values(1,'My First SQL book', 'Mary Parker', 981483029127, '2012-02-22 12:08:17'),
(2, 'My Second SQL book', 'John Mayer', 857300923713, '1972-07-03 09:22:45'),
(3, 'My Third SQL book', 'Cary Flint', 523120967812, '2015-10-18 14:05:44')

-- inserting values into reviews
insert into Reviews values(1, 1, 'John Smith', 'My first review', 4, '2017-12-10 05:50:11'),
(2, 2, 'John Smith', 'My second review', 5, '2017-10-13 15:05:12'),
(3, 2, 'Alice Walker', 'Another review', 1, '2017-10-22 23:47:10')

-- query to fetch details of books written by author whose name end with 'er'
select * from Books where author like '%er'

-- query to display title, author and reviewer name for all books
select title, author, reviewerName from Books b join Reviews r on b.id = r.bookId

-- query to display reviewer name who reviewed more than one book
select reviewerName from Reviews group by reviewerName having COUNT(bookId) > 1