#These files aim to use random sampling to get an idea of
#the odds for getting a particular set of stats for a
#Dungeons & Dragons character, given different rules such
# as rerolling 1's. This file provides analysis on the odds
# of getting a result that is "at least as good as" the
#array entered. 
#E.g. [16,14] would accept [17,14,14,12,9,6]
#as a success, since the highest two rolls are as good as
#the two rolls entered. [16,13,12,12,12,11] would fail.
mean(dataset2)
median(dataset2)

count_success<-function(topnum,minarray){
  success_count<-0
  for(i in 1:100000){
    local_success<-TRUE
    for(j in 1:topnum){
      if(dataset2[j,i]<minarray[j]){
        local_success<-FALSE
      }
    }
    if(local_success){
      success_count<-success_count+1
    }
  }
  return(success_count)
}

array1<-c(15,15)
results_15_15<-count_success(2,array1)
results_16_14<-count_success(2,c(16,14))
#/100000 to get the odds of rolling either of these at min


#Begin calculations for a character that can accept either
#[16,14], or [15,15]

#find specifically the (16+,14) case
minarray<-c(16,14)
success_count<-0
for(i in 1:100000){
  local_success<-TRUE
  
  if(dataset2[1,i]<16){
    local_success<-FALSE
  }
  if(dataset2[2,i]<14){
    local_success<-FALSE
  }
  if(dataset2[2,i]>14){
    local_success<-FALSE
  }
  
  if(local_success){
    success_count<-success_count+1
  }
}
success_count

final_odds<-(success_count+results_15_15)/100000
final_odds#the total odds of getting a character whose highest 2 stats
#are at least [16,14] or [15,15]

count_unique<-function(datameme,number){
  count<-0
  for(i in 1:100000){
    for(j in 1:6){
      if(datameme[j,i]==number){
        count<-count+1
      }
    }
  }
  return(count)
}

stats_frequency<-3:18
for(i in 3:18){
  stats_frequency[i-2]<-count_unique(dataset,i)
}

stats_frequency2<-3:18
for(i in 3:18){
  stats_frequency2[i-2]<-count_unique(dataset2,i)
}
