#include "F4.h"
#include <stdio.h>

int main()
{
  double x=0;
  double y=0;
  printf("x=");
  scanf("%lf",&x);
  printf("%lf\ny=",x);
  scanf("%lf",&y);
  printf("%lf\nresult=%lf\n",y,F4(x,y));
  return 0;
}
