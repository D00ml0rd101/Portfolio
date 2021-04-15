#idea is to create a check for palindrome, check from the highest down
#assume that the result in the 9##### range
#import numpy

def palincheck(number):
    check=False
    if number>99999:
        a=str(number)
        b=a[3:6]
        if b[2]==a[0] and b[1]==a[1] and b[0]==a[2]:
            check=True
    return check

bestguess=0#when all else fails, bounding exhaustive search
for i in range(1,1000):
    a=1000-i
    #num1=a*10+1
    for j in range(1,1000):
        b=1000-j
        #num2=b*10+9
        #tot=num1*num2
        tot=a*b
        if tot>bestguess:
            check=palincheck(tot)
        #print(tot)
            if check:
                bestguess=tot
            #break
    #if check:
        #break
        
