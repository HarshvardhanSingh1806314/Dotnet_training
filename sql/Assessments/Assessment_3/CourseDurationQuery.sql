use Assessment

create table CourseDetails(
	courseId char(5) unique,
	courseName varchar(20),
	startDate date,
	endDate date,
	fee int
)
 
-- inserting into course details table
insert into CourseDetails values('DN003', 'DotNet', '2018-02-01', '2018-02-28', 15000),
('DV004', 'DataVisualization', '2018-03-01', '2018-04-15', 15000),
('JA002', 'AdvancedJava', '2018-01-02', '2018-01-20', 10000),
('JC001', 'CoreJava', '2018-01-02', '2018-01-12', 3000)

-- creating function to calculate course duration
create function fn_CalculateDuration(@startDate date, @endDate date)
returns int
as
begin
	return datediff(day, @startDate, @endDate)
end

select *, dbo.fn_CalculateDuration(startDate, endDate) as courseDuration from CourseDetails