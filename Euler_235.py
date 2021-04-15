import math
#create array of size 5000 to store constants I solved for
alpha=[0]*5000

for k in range(1,5001):
    alpha[k-1]=900-(3*k)
    
def param_sweep_r(alpha,x,y,n):
    #alpha is are the constants, x and y are the values to sweep between, n is
    #how many points to sweep
    
    intervals=[0]*n
    for i in range(0,n):
        intervals[i]=x+(i*(y-x)/n)

    results=[0]*n
    for i in range(0,n):
        for j in range(0,5000):
            results[i]=results[i]+alpha[j]*math.pow(intervals[i],j)

    return results

#now perform newtons metod or rk4 using the approximation of r=1.0023 for the
#neighborhood to search around. Alternatively, cheese it using parameter sweep
#and some numerical method

#time for a really dirty numerical method?
accuracy=5
guessnow=1.00231
#guessprev=1.0023
goal=-600000000000
calc=0
prevcalc=0

def calc_alpha(alpha,r):
    output=0
    for j in range(0,5000):
            output=output+alpha[j]*math.pow(r,j)
    return output

while accuracy<14:
    prevcalc=calc
    calc=calc_alpha(alpha,guessnow)
    #determine direction to iterate in
    if abs(prevcalc-goal)<=abs(calc-goal):#if we overshot its true
            accuracy=accuracy+1
    if calc-goal>0:
        #guessprev=guessnow
        guessnow=guessnow+math.pow(10,-accuracy)
        #print(calc)
        #print(guessnow)
            
        #if (prevcalc
        #abs figure out if we overshot more than we got closer
        # if we did, increase accuracy then continue iterating
        #if we didn't, iterate in the correct direction
    else:
        #guessprev=guessnow
        guessnow=guessnow-math.pow(10,-accuracy)
    #print(calc)
    #print(guessnow)
    #print(accuracy)

print(calc)
print(guessnow)
#convergence method wins once again


    
