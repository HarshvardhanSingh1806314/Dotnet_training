use Assessment

-- creating student table
create table Students(
	registerNo int not null,
	studentName varchar(10) not null,
	age int not null,
	qualification varchar(6),
	mobileNo int,
	mailId varchar(20),
	loc varchar(10) not null,
	gender varchar(1) not null
)

-- inserting into students table
insert into Students values(2, 'Sai', 22, 'B.E', 9952836777, 'Sai@gmail.com', 'Chennai', 'M'),
(3, 'Kumar', 20, 'BSC', 7890125648, 'Kumar@gmail.com', 'Madurai', 'M'),
(4, 'Selvi', 22, 'B.Tech', 8904567342, 'selvi@gmail.com', 'Selam', 'F'),
(5, 'Nisha', 25, 'M.E', 7834672310, 'Nisha@gmail.com', 'Theni', 'F'),
(6, 'SaiSaran', 21, 'B.A', 7890345678, 'saran@gmail.com', 'Madurai', 'F'),
(7, 'Tom', 23, 'BCA', 8901234675, 'Tom@gmail.com', 'Pune', 'M')

-- displaying gender, total no of male and female students
select gender, COUNT(*) from Students group by gender