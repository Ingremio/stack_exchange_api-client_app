export default class HttpClient {
    public static async get<T>(url: string, config?: RequestInit): Promise<T> {
        const init = {
            method: 'get',
            ...config
        }
        return await this.http<T>(url, init);
    }

    public static async post<T, U>(url: string, body: T, config?: RequestInit): Promise<U> {
        const init = {
            method: 'post',
            body:  JSON.stringify(body),
            headers: {
                'Accept': 'application/json, text/plain',
                'Content-Type': 'application/json;charset=UTF-8'
            },
            ...config
        }

        return await this.http<U>(url, init);
    }

    public static async put<T, U>(url: string, body: T, config?: RequestInit): Promise<U> {
        const init = {
            method: 'put',
            body: JSON.stringify(body),
            headers: {
                'Accept': 'application/json, text/plain',
                'Content-Type': 'application/json;charset=UTF-8'
            },
            ...config
        }
        return await this.http<U>(url, init);
    }

    private static async http<T>(url: string, config: RequestInit): Promise<T> {
        const request = new Request(url, config);
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`${response.status} ${response.statusText}`)
        }

        return response.json().catch(() => ({}));
    }
}