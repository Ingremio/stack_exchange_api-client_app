export default class MathHelper {
    public static calculatePopularity(num: number, numbs: number[]): number {
        const maximum = this.findMaximum(numbs);
        const percentage = this.takePercentage(num, maximum);
        return this.roundNumber(percentage, 2);
    }

    public static findMaximum(numbs: number[]): number {
        return Math.max.apply(null, numbs);
    }

    public static takePercentage(nominator: number, denominator: number): number {
        return (nominator * 100) / denominator;
    }

    public static roundNumber(num: number, decimalPlaces = 0): number {
        if (num < 0) {
            return -this.roundNumber(-num, decimalPlaces);
        }

        const p = Math.pow(10, decimalPlaces);
        const n = num * p;
        const f = n - Math.floor(n);
        const e = Number.EPSILON * n;

        return (f >= .5 - e) ? Math.ceil(n) / p : Math.floor(n) / p;
    }
}