#These files aim to use random sampling to get an idea of
#the odds for getting a particular set of stats for a
#Dungeons & Dragons character, given different rules such
# as rerolling 1's. This file generates the data sets.
rolldice<-function(reroll_ones)
  for(x in 1:4){
    if(reroll_ones){#if we are rerolling ones
      rolls<-ceiling(runif(4)*5+1)
    }
    else{
      rolls<-ceiling(runif(4)*6)
    }
    #add largest 3
    rolls<-sort(rolls,decreasing = TRUE)
    output<-sum(rolls[1:3])
    return(output)
  }


set.seed(420)
dataset<-matrix(nrow = 6,ncol=100000)
reroll_ones=FALSE


for(sample in 1:100000){
  temp_list<-1:6
  for(stats in 1:6){
    temp_list[stats]<-rolldice(reroll_ones)
  }
  dataset[,sample]<-sort(temp_list,decreasing = TRUE)
}

set.seed(420)
dataset2<-matrix(nrow = 6,ncol=100000)
reroll_ones=TRUE

for(sample in 1:100000){
  temp_list<-1:6
  for(stats in 1:6){
    temp_list[stats]<-rolldice(reroll_ones)
  }
  dataset2[,sample]<-sort(temp_list,decreasing = TRUE)
}