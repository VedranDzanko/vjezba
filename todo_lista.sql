create database todo_lista;

create table todo_lista(
	sifra int not null primary key identity(1,1),
	naziv varchar(50),
	korisnik int,
);
create table korisnik(
	sifra int not null primary key identity(1,1),
	ime varchar(50),
	prezime varchar(50),
	korisnicko_ime varchar (50),
	lozinka char (8)
);
create table zadatak(
	sifra int not null primary key identity(1,1),
	naziv varchar (50),
	datum datetime,
	status bit,
	todo_lista int,
	kategorija int
);
create table kategorija(
	sifra int not null primary key identity(1,1),
	naziv varchar(50)
);

alter table zadatak add foreign key (kategorija) references kategorija(sifra);
alter table todo_lista add foreign key (korisnik) references korisnik(sifra);
alter table zadatak add foreign key (todo_lista) references todo_lista(sifra);

select * from korisnik
insert into korisnik (ime,prezime, korisnicko_ime, lozinka) values
('Vedran','Džanko','dzanko87', 'granicar'),
('Vedrana','Lukić','VL', 'nkrask69'),
('Toni','Perić','TL','nkjadran');

insert into todo_lista (naziv,korisnik) values
('Izgradnja vikendice', 2),
('Roštilj', 1),
('Turistički posjet Sofiji', 3);

insert into kategorija (naziv) values
('Projekt'),('Šoping'),('Team building');

insert into zadatak (naziv, datum, status, todo_lista, kategorija) values
('Izvadi građevinsku dozvolu',GETDATE(), 0, 1 ,1),
('Kupi meso', GETDATE(), 0, 2, 2),
('Kupi pivo', GETDATE(),0, 2, 2),
('Rezviraj hotel', GETDATE(), 0, 3, 3);


--pivo kupljeno
update zadatak set status = 1 where naziv = 'Kupi pivo';