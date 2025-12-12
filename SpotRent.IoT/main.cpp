#include <iostream>
#include <unistd.h>
#include <cstdio>

void loop();

int main() {
    printf("%s", "jlhbf");
    loop();

    return 0;
}

void loop() {
    int c = 0;
    while (true) {
        sleep(1);
        printf("%s no: %d\n", "Hell", ++c);
        fflush(stdout);
    }
}
