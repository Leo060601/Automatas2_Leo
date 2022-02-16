#include <stdio.h>
#include <conio.h>
#include <math.h>
#include <stdlib>
#include <graphics.h>

float x, y;


void main()
{
    //x=100;
    //printf("X es: %f", x);

    //y=100;

    printf("\n\tX: ");
    scanf("%d", &x);
    printf("\n\tY: ");
    scanf("%d", &y);
    printf("\n\ty = %f  x = %f\n", y, x);
    if (x >= 100)
    {
        printf("\tIf 1\n");
        if (y == 100)
        {
            printf("\tIf 1.1\n");
        }
        else
        {
            printf("\telse 1.1\n");
        }
    }
    if (!x <= 100)
    {
        if (z == 30)
            printf("\tIf 2\n");
    }
    do
    {
        printf("\nEste es el Do While\n");
    } while (x > 100);
}