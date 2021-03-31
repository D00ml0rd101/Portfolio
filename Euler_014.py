
clength=[0]*1000000#list for shortcutting chain information
clength[0]=1
largestval=1
largestindex=0
for i in range(2,1000000):
    chaining=True
    temp=i
    chains=0
    while chaining:
        if temp%2==0:
            temp=temp/2
        else:
            temp=3*temp+1

        if temp<i:
            #record result from chain, since we are now at a known number
            chains+=clength[int(temp-1)]
            clength[int(i-1)]=chains
            chaining=False
            #records best result
            if chains>largestval:
                largestval=chains
                largestindex=i-1
        else:
            chains+=1
