module Cb {
    Array.prototype.add = function(this : Array<any>, element: any) {
        this.push(element);
    }

    Array.prototype.remove = function(this : Array<any>, element?: any) {
        let index = this.length - 1;
        if(element) index = this.indexOf(element);
        this.removeAt(index);
    }

    Array.prototype.removeAt = function(this : Array<any>, index: number) {
        this.splice(index, 1);
    }
}
