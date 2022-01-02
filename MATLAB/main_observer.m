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
C = [1 0 0 0;
    0 0 0 1];

Q = diag([10 1 100 1]);
R = 1;

K = lqr(A, B, Q, R);
L = lqr(A', C', eye(size(A)), eye(size(C, 1))*1e-1)';

u_old = 0;
u = 0;

t_old = nan;
y_old = nan;
xhat = zeros(size(A, 1), 1);
logger = tools.logger('xhat', 'x', 'u', 't');
for k = 1:length_control
    set_input(u, set_port);
    [x, y, t] = get_information(get_port);
    if ~any(isnan(y)) && ~any(isnan(y_old)) && ~isnan(t_old)
        dt = t-t_old;
        k1 = A*xhat + B*u_old + L*(y_old-C*xhat);
        xhat1 = xhat + dt*k1/2;
        y1 = (y_old+y)/2;
        k2 = A*xhat1 + B*u_old + L*(y1-C*xhat1);
        xhat2 = xhat + dt*k2/2;
        k3 = A*xhat2 + B*u_old + L*(y1-C*xhat2);
        xhat3 = xhat + dt*k3;
        k4 = A*xhat3 + B*u_old + L*(y-C*xhat3);
        xhat = xhat + dt*(k1+2*k2+2*k3+k4)/6;
    end
    u_old = u;
    u = -K*xhat;
    t_old = t;
    y_old = y;
    if ~any(isnan(x))
        logger.add_data('xhat', xhat);
        logger.add_data('x', x);
        logger.add_data('t', t);
        logger.add_data('u', u);
    end
end

state_names = {'Position', 'Velocity', 'Angle', 'AnglarVelocity'};
data = logger.get_logs();
for k =1:numel(state_names)
   figure, plot(data.t, [data.x(:, k), data.xhat(:, k)]);
   xlabel('Time')
   ylabel(state_names{k})
end
figure, plot(data.t, data.u);