create or replace function public.populateTestCase() returns void as $$
begin
  delete from account;
  delete from owner; 
  delete from portfolio;
  
  insert into owner (id, owner_name) values (1,"Tom");
  insert into owner (id, owner_name) values (2,"Bob");
  insert into owner (id, owner_name) values (3,"Harry");

  insert into portfolio (id, port_name) values (1, "One");
  insert into portfolio (id, port_name) values (2, "Two");
  insert into portfolio (id, port_name) values (3, "Three");
  
  insert into account (id, portfolio_id, owner_id, account_number, balance) 
               values (1, 1, 2, 'ABC123', 100.00);
  insert into account (id, portfolio_id, owner_id, account_number, balance) 
               values (2, 2, 3, 'DEF456', 200.00);
  insert into account (id, portfolio_id, owner_id, account_number, balance) 
               values (3, 3, 2, 'GHI789', 300.00);
  insert into account (id, portfolio_id, owner_id, account_number, balance) 
               values (4, 2, 3, 'JKL012', 400.00);
  insert into account (id, portfolio_id, owner_id, account_number, balance) 
               values (5, 1, 3, 'MNO345', 500.00);
end;
$$ language plpgsql;
