/* SEARCH fill in standard questions */
trace n
signal on novalue
erc = 0
parse arg type 
select
  when type = "I" then do
     lf = x2c("0D0A")
     r = 0
     do while r < 1 | r > 2
       say "Select the question" lf "1 Mail to be handled within 7 days" lf "2 Mail of last week"
       pull r
     end
     if r=1 then do
        "S 1 | >= AD" day(0)
        "S 2 & <= AD" day(7)
        "S 3 & = AT n" 
        "SEARCH" 
        if rc = 0 then "SHOW" 
      end
     if r=2 then do
        "S 1 | >= DR" day(-8)
        "S 2 & <= DR" day(-1)
        "SEARCH" 
        "SEARCH" 
        if rc = 0 then "SHOW" 
      end
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
