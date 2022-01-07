function [x, y, t] = get_information(Port, wait_time)
if nargin < 2
    wait_time = 20;
end
MsgLength = 1000;
try
msg = judp('RECEIVE', Port, MsgLength, wait_time);
dat = jsondecode(char(msg'));
x = [dat.position_car; dat.velocity_car; dat.angle_pendulum; dat.angular_velocity_pendulum];
y = [dat.position_car; dat.angular_velocity_pendulum];
t = dat.time;
catch
   x = nan(4, 1);
   y = nan(2, 1);
   t = nan;
   msg = [];
end
fprintf('dat: %s:%s\n', datestr(now, 'HH:MM:SS.FFF'), char(msg'));
end