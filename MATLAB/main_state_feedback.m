clear
close all

get_port = 8000;
set_port = 22222;
length_control = 1e4;

M = 1;
m = 5;
L = 3;
W = 0.1;
l = L/2;
J = 1/12*m*(L^2+W^2);
g = 9.81;

E = [0 m+M 0 m*l;
    0 m*l 0 m*l^2+J;
    1 0 0 0;
    0 0 1 0];
A = E\[0 0 0 0;
    0 0 m*g*l 0;
    0 1 0 0;
    0 0 0 1];

B = E\[1; 0; 0; 0];

Q = diag([10 1 100 1]);
R = 1;

K = lqr(A, B, Q, R);

u = 0;

for k = 1:length_control
set_input(u, set_port);
[x, y] = get_information(get_port);

u = -K*x;

end