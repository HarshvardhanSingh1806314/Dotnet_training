-- printing my birthday (day of week)
select DATENAME(WEEKDAY, '1999-04-28') as birthday_day

-- query to display my age in days
select DATEDIFF(day, '1999-04-28', GETDATE()) as age_in_days

-- query to display all employees information who joined before 5 years in the current month
use Assignments
select * from Employees where DATEDIFF(year, hireDate, GETDATE()) > 5 and MONTH(GETDATE()) = MONTH(hireDate)