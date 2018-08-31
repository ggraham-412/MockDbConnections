create table if not exists public.owner (
    id serial primary key,
	owner_name varchar(32) not null
);

create table if not exists public.portfolio (
    id serial primary key,
	port_name varchar(32) not null
);

create table if not exists public.account (
    id serial primary key,
	portfolio_id int references public.portfolio(id) not null on delete cascade,
	owner_id int references public.owner(id) not null on delete cascade,
    account_number varchar(6),
    balance numeric(12,4)	
);

