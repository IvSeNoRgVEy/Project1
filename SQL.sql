--create database Press;
use Press;
--create table Publishing
--(
--	[Id] int not null identity(1,1) primary key,
--	[Name_PUBL] nvarchar(50)
--)
--create table Type
--(
--	[Id] int not null identity(1,1) primary key,
--	[Name_TP] nvarchar(50)
--)
--create table Country
--(
--	[Id] int not null identity(1,1) primary key,
--	[Name_CNTR] nvarchar(50)
--)


--insert into Country
--values
--('������'),
--('���');

--insert into Type
--values
--('������'),
--('������'),
--('������'),
--('�����'),
--('�������');

--insert into Publishing
--values
--('URSS.ru'),
--('�����-����'),
--('��������'),
--('�����������'),
--('Hachette Book Group'),
--('HarperCollins'),
--('Simon & Schuster'),
--('Penguin Random House');

--create table Products
--(
--	[Id] int not null identity(1,1) primary key,
--	[Name] nvarchar(50),
--	[Circulation] int,
--	[Price] money,
--	[Period] nvarchar(50),
--	[Demand] int,
--	[Publishing_FK] int foreign key([Publishing_FK]) references Publishing(ID),
--	[Type_FK] int foreign key([Type_FK]) references Type(ID),
--	[Country_FK] int foreign key([Country_FK]) references Country(ID)
--)

--insert into Products
--values
--('������',5000,50,'�������',85,1,1,1),
--('���������',2000,90,'���',90,1,1,1),
--('�������� ������',3500,120,'��������',75,2,1,1),
--('��������',4000,100,'���',58,2,1,1),
--('5 ������',1000,200,'����',60,3,2,1),
--('������� �������',2000,300,'�������',65,3,2,1),
--('������ ���',2500,330,'�������',70,4,2,1),
--('����� 2033',4000,550,'����',90,4,4,1),
--('��� ���������',6000,800,'����',80,3,4,1),
--('������',1000,100,'�������',65,1,4,1),
--('��-��',1500,230,'�������',70,1,4,1),
--('� ���� ��������',1550,55,'������',95,1,3,1),
--('� �������',2000,45,'������',94,3,3,1),
--('� ����� �����',1060,100,'������',98,2,3,1),
--('�������� ',1000,550,'�������',77,2,5,1),
--('������������',900,405,'������',84,3,5,1),
--('�������������',1000,500,'������',58,1,5,1),
--('���������������',1500,630,'����',68,1,5,1),
--('����� �������',3000,500,'�������',85,5,1,2),
--('Time',5000,120,'���',90,6,1,2),
--('New York now',3500,220,'��������',75,7,3,2),
--('Forbes',2000,600,'���',65,8,2,2),
--('Happy New Year',10000,80,'�������',95,6,3,2),
--('�����-�������',1000,700,'�������',65,5,5,2),
--('PLAYBOY',2500,1000,'������',90,8,2,2);

select Name_TP,Name,Name_PUBL,Name_CNTR,Circulation,Price
from Products,Type,Publishing,Country
where Name_TP='������' and Type.Id=Type_FK and Publishing.Id=Publishing_FK and Country.Id=Country_FK
order by Name;
select Name as [��������],Circulation as [�����],Price as [���� �� ��.],Period as [����� �������],Name_TP as [���],Name_PUBL as [������������],Name_CNTR as [������]
from Products,Type,Publishing,Country
where Type.Id=Type_FK and Publishing.Id=Publishing_FK and Country.Id=Country_FK;

