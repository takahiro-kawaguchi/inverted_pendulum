function set_input(u, port)

dat = struct();
dat.u = u;
judp('send', port, 'localhost',int8(jsonencode(dat)));

end

