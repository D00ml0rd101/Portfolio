fib1=1
fib2=2
fibnext=0
count=0
sum=2
#the idea was that fibonaaci follows an odd odd even pattern
while fibnext<=4000000:
    fibnext=fib1+fib2
    count=count+1
    if count%3==0:
        sum=sum+fibnext
    fib1=fib2
    fib2=fibnext
    
    
