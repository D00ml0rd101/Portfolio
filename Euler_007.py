import math
plist=[0]*10002
#this method is designed for large primes, so I need to have the first few
#in already
plist[0]=2
plist[1]=3
plist[2]=5
pcount=3
plarge=3
pnext=1

count=5
while pcount<10001:
    count=count+1
    check=True #check whether it is a prime number
    #update the largest prime to be considered for factors
    if math.sqrt(count)<plist[pnext]:
        pnext=pnext+1
    for i in range(0,pnext):
        if (count/plist[i])%1==0:
            check=False
            break
        
    if check:
        plist[pcount]=count
        pcount=pcount+1
        #print(pcount)

print(plist[10000])
