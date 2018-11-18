/* REGISTER Create standard registers */
trace n
signal on novalue
erc = 0
parse arg type 
select
  when type = "I" then do
     lf = x2c("0D0A")
     r = 0
     do while r < 1 | r > 3
       a = "Select the register" 
       a = a || lf "1 Incoming mail per receiver"
       a = a || lf "2 Incoming mail per handler"
       a = a || lf "3 Outgoing mail per sender"
       say a
       pull r
     end
     if r=1 then do
        "REGEXP .*"
        "R Inco To"
        "CREATE" 
        if rc = 0 then "SHOW Inco rTo"  
      end
     if r=2 then do
        "REGEXP .*"
        "R Inco Ap"
        "CREATE" 
        if rc = 0 then "SHOW Inco rAp"  
      end
     if r=3 then do
        "REGEXP .*"
        "R Outg Snd"
        "CREATE" 
        if rc = 0 then "SHOW Outg rSnd" 
      end
  end
end
exit erc
