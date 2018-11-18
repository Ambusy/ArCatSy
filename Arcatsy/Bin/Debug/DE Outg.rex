/* DE Outg */
trace n
signal on novalue
text="TEXT"
visible="VISIBLE"
erc = 0
parse arg entry type fname
select
  when type = "D" then do
    "SIZE 10 20 700"
    "DEFINE To T 1 100 n n To"
    "DEFINE Snd T 1 100 n n Sender"
    "DEFINE Cc T 9 100 n n Copies"
    "DEFINE Ds T 1 10 n n Date sent"
    "DEFINE Ref T 1 100 n n Our reference"
    "DEFINE Your T 1 100 n n Your reference"
    "DEFINE Sj T 1 255 n n Subject"
    "DEFINE Src T 1 255 n n Source"
    "SYSMSG 357"
    imsg = sysmsg
    "SYSMSG 358"
    mmsg = sysmsg
  end
  when type = "I" then do
    "INITDATA"
    "GETDATA msg Ds"
    msg.text = imsg
    Ds.1.text = Day(0) 
    "PUTDATA msg Ds"
  end
  when type = "K" then do
    "SELECTKEY To,Snd ALL"
    if rc = 1 then do     
       "INITDATA"
       "GETDATA msg Ds"
       msg.text = imsg
       Ds.1.text = Day(0) 
       "PUTDATA msg Ds"
    end
    else if rc = 0 then do
       "READDATA"
       "GETDATA msg Ds"
       msg.text = mmsg
       "PUTDATA msg"
    end
  end
  when type = "N" then do 
    "READDATA"
    msg.text = mmsg
    "PUTDATA msg"
  end
  when type = "S" then do
    "SAVE"
  end
end
exit erc
 
Day:
  parse arg n
  dt = Date("S")     
  j = substr(dt,1,4)
  m = substr(dt,5,2)     
  d = substr(dt,7,2)   
  if n <> 0 then do
     d = d + n
     call mxD
     do while d > mx  
        d = d - mx
        m = m + 1
        if m > 12 then do
           m = 1
           j = j + 1
        end
        call mxD
     end
     do while d < 0
        m = m - 1
        if m = 0 then do
           m = 12
           j = j - 1
        end
        call mxD
        d = d + mx
     end
  end
  dt = j || "/" || right("0"||m,2) || "/" || right("0"||d,2)  
  return dt
 
mxD:  
  l = j // 4
  if m = 4| m = 6 | m = 9 | m = 11 then mx = 30
  else if m <> 2 then mx = 31
  else if l = 0 then mx = 29
  else mx = 28
  return  
