/* Edit text before seaching, returns variable T */
trace n
signal on novalue
parse arg TermCode TermText
t = TermText
upper TermCode
if pos(TermCode,"DT DR DS AD ")>0 then do 
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
   exit 0
end
exit 4
