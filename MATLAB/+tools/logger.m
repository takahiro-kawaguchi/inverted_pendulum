classdef logger < handle
    properties
        params
        logs
    end
    
    methods
        function obj = logger(varargin)
            obj.params = {};
            obj.logs = struct();
            if nargin ~= 0
                obj.add_params(varargin{:});
            end
        end
        
        function add_params(obj, varargin)
            if nargin == 3
                if isnumeric(varargin{end})
                    if iscell(varargin{1})
                        params = varargin{1}; %#ok
                        for itr = 1:numel(params) %#ok
                            obj.add_params(params{itr}, varargin{end}); %#ok
                        end
                    else
                        if ~ismember(varargin{1}, obj.params)
                            obj.params = [obj.params; varargin(1)];
                            obj.logs.(varargin{1}) = tools.local_logger(varargin{2});
                        end
                        
                    end
                else
                    obj.add_params(varargin, []);
                end
            else
                if isnumeric(varargin{end})
                    obj.add_params(varargin(1:end-1), varargin{end});
                else
                    obj.add_params(varargin, []);
                end
            end
        end
        
        function add_data(obj, varargin)
            if isstruct(varargin{1})
                p = fieldnames(varargin{1});
                for itr = 1:numel(p)
                    v = varargin{1}.(p{itr});
                    obj.logs.(p{itr}).add_data(v);
                end
            else
                if mod(numel(varargin),2) == 0
%                     d = struct();
                    for itr = 1:2:numel(varargin)
%                         d.(varargin{itr}) = varargin{itr+1};
                          obj.logs.(varargin{itr}).add_data(varargin{itr+1});
                    end
%                     obj.add_data(d);
                end
            end
        end
        
        function log = get_logs(obj, params, Start, End)
            get_one = false;
            if nargin > 1
                if ischar(params)
                    params = {params};
                    get_one = true;
                end
                if ~iscell(params)
                    if nargin > 2
                        End = Start;
                    else
                        End = [];
                    end
                    Start = params;
                    params = obj.params;
                else
                    if nargin < 4
                        End = [];
                    end
                    if nargin < 3
                        Start = [];
                    end
                end
            else
                params = obj.params;
                Start = [];
                End = [];
            end
            log = struct();
            for itr = 1:numel(params)
                p = params{itr};
                log.(p) = obj.logs.(p).get_log(Start, End);
            end
            if get_one
                log = log.(params{1});
            end
        end
        
    end
    
    
end
