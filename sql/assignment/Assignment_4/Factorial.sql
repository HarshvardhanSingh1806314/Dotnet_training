-- creating a function to calculate factorial of a number

create function fn_calculateFactorial(@number int)
returns int
as
begin
	declare @factorial int = 1
	if @number < 0
	begin
		return null
	end
	else if @number > 1
	begin
		while @number > 0
		begin
			set @factorial = @factorial * @number
			set @number = @number - 1
		end
	end
	return @factorial
end

select dbo.fn_calculateFactorial(5) as factorial