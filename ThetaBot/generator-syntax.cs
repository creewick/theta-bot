for(var $var$=0; $var$<n; $var$ $inc$)
   c++;

var: one of [i, k, j, index]
inc: one of [++ +=1 +=2 +=3]
Θ(n)

for(var i=0; i<$mult$*n; i $inc$)
   c++;

mult: one of [2 10 100]
inc: one of [++ +=1 +=2 +=3]
Θ(n)

for(var i=$mult$*n; i>=0; i $dec$)
   c++;

mult: one of [2 10 100]
dec: one of [-- -=1 -=2 -=3]
Θ(n)


for(var i=0; i<n*n; i $inc$)
   c++;

inc: one of [++ +=1 +=2 +=3]
Θ(n²)

for(var i=1; i<$mult$n; i $inc$)
   c++;

mult: one of ["" "2*" "10*"]
inc: one of [*=2 *=10 <<1 +=i]
Θ(log n)


for(var i=1; i<n*n; i $inc$)
   c++;

inc: one of [*=2 *=10 <<1 +=i]
Θ(log n)

logn с делением
========================

for(var i=1; i<$mult$n; i $inc$)
    for(var j=1; i<$mult$n; i $inc$)
        c++;
