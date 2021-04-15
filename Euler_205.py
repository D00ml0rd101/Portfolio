import math
P=[0]*(36-8)
C=[0]*(36-5)

#find the occurences and combinations of the 9d4s

for d41 in range(0,4):
    for d42 in range(0,4):
        for d43 in range(0,4):
            for d44 in range(0,4):
                for d45 in range(0,4):
                    for d46 in range(0,4):
                        for d47 in range(0,4):
                            for d48 in range(0,4):
                                for d49 in range(0,4):
                                    index=d41+d42+d43+d44+d45+d46+d47+d48+d49
                                    P[index]=P[index]+1/math.pow(4,9)

#same for the 6d6's
for d61 in range(0,6):
    for d62 in range(0,6):
        for d63 in range(0,6):
            for d64 in range(0,6):
                for d65 in range(0,6):
                    for d66 in range(0,6):
                        index=d61+d62+d63+d64+d65+d66
                        C[index]=C[index]+1/math.pow(6,6)

pcumulative=0                                  
for i in range(0,36-5):
    sumPWin=0
    for j in range(max(0,i-2),36-8):
        sumPWin=sumPWin+P[j]
        #print(j)
    pcumulative=pcumulative+C[i]*sumPWin

print(pcumulative)
