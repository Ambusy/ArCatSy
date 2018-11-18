/* Indexing a text for: X (index) or R (register) */
trace n
signal on novalue
parse arg T CollCode FldCode text
if FldCode = "0" then do 
   t.0 = 1 
   parse var text . t.1  /* only seqnr */
end 
else do 
   t.0 = words(text)
   do i=1 to t.0
     t = word(text,i) 
     if t = "R" then do
       t1 = substr(t,1,1)
       t2 = substr(t,2) 
       upper t1 
       lower t2 
       t.i = t1 || t2 
     end
     else do
       upper t
       t.i = t 
     end
   end 
end
exit 0
