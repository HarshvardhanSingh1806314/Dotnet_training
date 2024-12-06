use Assignments

create function fn_studentPassOrFail(@studentMarks int)
returns varchar(10)
as
begin
	declare @result varchar(10)
	if @studentMarks >= 50
	begin
		set @result = 'pass'
	end
	else
	begin
		set @result = 'fail'
	end
	return @result
end

-- creating student table
create table student (
	studentId int primary key,
	studentName varchar(20)
)

-- inserting into student table
insert into student values(1, 'Jack'), (2, 'Rithvik'),
(3, 'Jaspreeth'), (4, 'Praveen'), (5, 'Bisa'), (6, 'Suraj')

-- creating marks table
create table marks (
	marksId int primary key,
	studentId int references student(studentId),
	score int
)

-- inserting into marks table
insert into marks values(1, 1, 23), (2, 6, 95),
(3, 4, 98), (4, 2, 17), (5, 3, 53), (6, 5, 13)

select s.studentId, s.studentName, m.score, dbo.fn_studentPassOrFail(m.score) as status
from student s join marks m on s.studentId = m.studentId order by s.studentId