/* Edit text before printing, returns variable T */
trace n
signal on novalue
parse arg CollCode FldCode seqnr text
t = text
if FldCode = "Src" then do /* SRC becomes a reference to the file */
   i = lastpos("\", text) 
   if i > 0 then 
      dsn = substr(text,i+1)
   else
      dsn = text
   t = '<a href="' || text || '">' || dsn || '</a>'
end 
exit 0
