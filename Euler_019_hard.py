#this was the one used to submit. less efficient but no error
days=[31,28,31,30,31,30,31,31,30,31,30,31]
ldays=[31,29,31,30,31,30,31,31,30,31,30,31]
monthday=2#first day of a month, 0 is a sunday
scount=0

for i in range(1,101):
    if i%4==0:#leap year
        for j in range(0,12):
            if monthday==0:
                scount+=1
            monthday=(monthday+ldays[j])%7
    else:
        for j in range(0,12):
            if monthday==0:
                scount+=1
            monthday=(monthday+days[j])%7
print(scount)
