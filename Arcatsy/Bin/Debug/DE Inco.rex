/* DE Inco */
trace n
signal on novalue
text="TEXT"
visible="VISIBLE"
erc = 0
parse arg entry type fname
select
  when type = "D" then do
    "SIZE 10 20 700"
    "DEFINE Snd T 1 100 n n Sender"
    "DEFINE To T 1 100 n n Adressed to"
    "DEFINE Cc T 9 100 n n Copies"
    "DEFINE Ap T 5 100 n y Action by"
    "DEFINE Ad Ap T 1 10 n y Action before"
    "DEFINE At Ap T 1 1 n n Action taken?"
    "DEFINE Ref T 1 50 n n Reference"
    "DEFINE Our T 1 50 n n Our reference"
    "DEFINE Ds T 1 10 n y Date sent"
    "DEFINE Dr T 1 10 n y Date received"
    "DEFINE Sj T 1 255 n n Subject"
    "SYSMSG 357"
    imsg = sysmsg
    "SYSMSG 358"
    mmsg = sysmsg
  end
  when type = "I" then do
    "INITDATA"
    "GETDATA msg Dr"      /* screen -> Rexx */
    msg.text = imsg
    Dr.1.text = Day(0) 
    "PUTDATA msg Dr"      /* Rexx -> screen */
  end
  when type = "K" then do        
    "SELECTKEY Snd,To ALL"
    if rc = 1 then do
       "INITDATA"
       "GETDATA msg Dr"
       msg.text = imsg
       dr.1.text = Day(0) 
       "PUTDATA msg Dr"
    end
    else if rc = 0 then do
       "READDATA"
       "GETDATA msg"
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
  when type = "L" then do  /* user completed field */
    fld = left(fname,3)
    if fld = "Ap." then do
       i = substr(fname,4)
       "GETDATA Ap At Ad msg"
       if value("Ap." || i || ".TEXT") <> "" then do
          if value("At." || i || ".1.TEXT") = "" then do
              interpret "At." || i || '.1.TEXT = "n"'
          end
          if value("Ad." || i || ".1.TEXT") = "" then do
              dt = day(+7)     
              interpret "Ad." || i || '.1.TEXT = "' || dt || '"'
          end
       end
       if value("Ap." || i || ".TEXT") = "ERROR" then do
          msg.text = "This is an error"
          erc = 1
       end
       "PUTDATA At Ad msg"
    end
    else if pos(fld,"Ad.Dr.Ds.") > 0  then do
       "GETDATA" left(fname,2)
       t=value(fname||".TEXT")
       if strip(t) <> "" then do
          call EditDate
          interpret fname || '.TEXT = "' || t || '"'
          "PUTDATA" left(fname,2)
       end
    end
  end
end
exit erc
  
Day: trace n
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
 
mxD: trace n 
  l = j // 4
  if m = 4| m = 6 | m = 9 | m = 11 then mx = 30
  else if m <> 2 then mx = 31
  else if l = 0 then mx = 29
  else mx = 28
  return
 
EditDate: trace n
 /* dates in format: d m y | d m | yyyy m d */ 
 /*                  d-m-y | d-m | yyyy-m-d */ 
 /*                  d/m/y | d/m | yyyy/m/d */ 
   t=translate(t,"  ","-/")                                     
   if words(t) = 2 then do  
      parse var t d m . 
      y = left(date("S"),4)
   end
   else parse var t d m y .
   if length(d) = 4 then do
      x = d  
      d = y
      y = x
   end
   if y < 99 then y = y + 2000
   if y < 999 then y = y + 1000
   t = y || "/" || right("0"||m,2) ||  "/" || right("0"||d,2)  
   return  
