create database todo_lista;
go

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