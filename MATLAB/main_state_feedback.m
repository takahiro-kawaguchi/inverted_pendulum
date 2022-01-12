clear
close all

get_port = 22223;
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
logger = tools.logger('x', 'u', 't');

for k = 1:length_control
    set_input(u, set_port);
    [x, y, t] = get_information(get_port);
    
    if ~any(isnan(x)) && t-t_old ~= 0
        u = -K*x;
        logger.add_data('x', x);
        logger.add_data('t', t);
        logger.add_data('u', u);
    end
    t_old = t;
end


state_names = {'Position', 'Velocity', 'Angle', 'AnglarVelocity'};
data = logger.get_logs();
for k =1:numel(state_names)
   figure, plot(data.t, data.x(:, k));
   xlabel('Time')
   ylabel(state_names{k})
end
figure, plot(data.t, data.u);