cycle=[3,5,6,9,10,12,15,18,20,21,24,25,27,30]#a complete cycle of 3 and 5 multiples
sum=0
for loops in range(0,34):  
    if not (loops==33):
        for i in cycle:
            sum=sum+i+30*loops
            #if loops==32:
            print(i+30*loops)
    else:
        for i in cycle[0:4]:
            sum=sum+i+30*loops
            print(i+30*loops)
        
        
