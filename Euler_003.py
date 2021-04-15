#gonna find all prime factors because this interested me
def smallprime(number,factorlist):
    numtemp=0.5
    count=1
    while not numtemp%1==0:
        count=count+1
        numtemp=number/count
    
    factorlist.append(count)
    return numtemp


factorlist=[]
number=600851475143
while not number==1:
    number=smallprime(number,factorlist)
print(factorlist)
