/* */ 
trace n
'top' 
'down 1' 
do while rc=0 
  'extract /curline/' 
  if curline.3 = '#Const ODBC = False' then do
       'del 1'  
       'extract /curline/'
     end
  if curline.3 = '#Const SqlServer = False' then do
       'del 1'  
       'extract /curline/'
     end
  if curline.3 = '#If ODBC Then' then do
     'del 1'
     'extract /curline/'
     do while left(curline.3,1) <> '#'
       'down 1'  
       'extract /curline/'
     end
     do while curline.3 <> '#End If'
       'del 1'  
       'extract /curline/'
     end
     'del 1'  
  end 
  else do
    'down 1' 
  end
end 
exit 