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
--('Россия'),
--('США');

--insert into Type
--values
--('Газета'),
--('Журнал'),
--('Буклет'),
--('Книга'),
--('Словарь');

--insert into Publishing
--values
--('URSS.ru'),
--('ГРАНД-ФАИР'),
--('Академия'),
--('Академкнига'),
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
--('Правда',5000,50,'Октябрь',85,1,1,1),
--('Ведомости',2000,90,'Май',90,1,1,1),
--('Вечерняя Москва',3500,120,'Сентябрь',75,2,1,1),
--('Известия',4000,100,'Май',58,2,1,1),
--('5 КОЛЕСО',1000,200,'Июль',60,3,2,1),
--('КАРАВАН ИСТОРИЙ',2000,300,'Декабрь',65,3,2,1),
--('ШИШКИН ЛЕС',2500,330,'Октябрь',70,4,2,1),
--('Метро 2033',4000,550,'Март',90,4,4,1),
--('Три мушкетера',6000,800,'Июнь',80,3,4,1),
--('Сказки',1000,100,'Декабрь',65,1,4,1),
--('Му-му',1500,230,'Октябрь',70,1,4,1),
--('С днем рождения',1550,55,'Апрель',95,1,3,1),
--('С юбилеем',2000,45,'Август',94,3,3,1),
--('С новым годом',1060,100,'Ноябрь',98,2,3,1),
--('Толковый ',1000,550,'Февраль',77,2,5,1),
--('Православный',900,405,'Август',84,3,5,1),
--('Академический',1000,500,'Ноябрь',58,1,5,1),
--('Орфографический',1500,630,'Март',68,1,5,1),
--('Голос Америки',3000,500,'Октябрь',85,5,1,2),
--('Time',5000,120,'Май',90,6,1,2),
--('New York now',3500,220,'Сентябрь',75,7,3,2),
--('Forbes',2000,600,'Май',65,8,2,2),
--('Happy New Year',10000,80,'Декабрь',95,6,3,2),
--('Англо-Русский',1000,700,'Декабрь',65,5,5,2),
--('PLAYBOY',2500,1000,'Август',90,8,2,2);

select Name_TP,Name,Name_PUBL,Name_CNTR,Circulation,Price
from Products,Type,Publishing,Country
where Name_TP='Журнал' and Type.Id=Type_FK and Publishing.Id=Publishing_FK and Country.Id=Country_FK
order by Name;
select Name as [Название],Circulation as [Тираж],Price as [Цена за ед.],Period as [Месяц продажи],Name_TP as [Тип],Name_PUBL as [Издательство],Name_CNTR as [Страна]
from Products,Type,Publishing,Country
where Type.Id=Type_FK and Publishing.Id=Publishing_FK and Country.Id=Country_FK;

