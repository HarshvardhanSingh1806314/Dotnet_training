use Assessment

-- creating product details table
create table ProductDetails(
	productID int primary key identity,
	productName varchar(30),
	price float,
	discountedPrice float
)

-- creating procedure to insert into ProductDetails table
create or alter proc sp_InsertProducts(@productName varchar(30), @price float, @discountedPrice float)
as
begin
	insert into ProductDetails values(@productName, @price, @discountedPrice)
end

drop table ProductDetails