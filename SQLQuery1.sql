SELECT * 
from animais

insert into obitos(nanimal)
select nanimal
from animais

delete 
from obitos
where nobito = 3