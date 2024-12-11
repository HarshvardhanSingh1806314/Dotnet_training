use Assessment

-- creating course info table
create table T_CourseInfo(
	courseName varchar(20),
	startDate date
)

-- creating trigger that will display the course name and start date when new course is inserted into course details table
create trigger tr_DisplayCourseNameAndStartDate
on CourseDetails after insert
as
begin
	-- inserting course name and start date into T_CourseInfo table
	insert into T_CourseInfo (courseName, startDate)
	select courseName, startDate from inserted
end

insert into CourseDetails values('DM001', 'DatabaseSystem', '2024-12-11', '2024-12-25', 15000)
select * from T_CourseInfo