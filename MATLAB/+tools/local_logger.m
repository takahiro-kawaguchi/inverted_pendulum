classdef local_logger < handle
    
    properties
        X = [];
        N=1000;
        n = 0;
    end
    
    methods
        function obj = local_logger(N)
            if nargin == 1 && ~isempty(N)
                obj.N = N;
            end
            obj.n = 0;
            obj.X = [];
        end
        
        function add_data(obj, x)
            s = size(x);
            if obj.n == 0
                if s(1) == 1 || s(2) == 1
                    obj.X = zeros(obj.N, numel(x));
                else
                    obj.X = zeros(obj.N, s(1), s(2));
                end
            elseif obj.n==size(obj.X, 1)
                %                tmp = obj.X;
                %                obj.X = zeros(size(obj.X,1)+obj.N, numel(x));
                %                obj.X(1:obj.n, :) = tmp;
                sX = size(obj.X);
                
                sX(1) = obj.N;
                obj.X = [obj.X; zeros(sX)];
            end
            %             x = x(:);
            sX = size(obj.X);
            
            if numel(sX) == 2
                x = x(:);
                obj.X(obj.n+1, :) = x';
            else
                obj.X(obj.n+1, :, :) = x;
            end
            obj.n = obj.n+1;
        end
        
        function d = get_log(obj, Start, End)
            if nargin < 2 || isempty(Start)
                Start = 1;
            end
            if nargin < 3 || isempty(End)
                End = obj.n;
            end
            sX = size(obj.X);
            if Start > obj.n
                n_nan = End-Start+1;
            else
                n_nan = max(0, End-obj.n);
            end
            if numel(sX) == 2
                d = [obj.X(Start:min(obj.n, End), :); nan(n_nan, size(obj.X, 2))];
            else
               d = [obj.X(Start:min(obj.n, End), :, :); nan(n_nan, size(obj.X, 2), size(obj.X, 3))];
               if size(d, 1) == 1
                   d = squeeze(d);
               end
            end
        end
        
    end
    
end

