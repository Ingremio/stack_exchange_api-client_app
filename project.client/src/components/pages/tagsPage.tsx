import { useEffect, useState } from 'react';
import PaginationComponent from 'react-reactstrap-pagination';
import { Container, Table } from 'reactstrap';
import HttpClient from '../helpers/httpClient';
import MathHelper from '../helpers/mathHelper';
import { IPagination } from '../reducers/interfaces/pagination';
import { Collective, Convert, Tags } from '../reducers/interfaces/tags';

export default function TagsPage() {
    const pageSize = 25;
    const [page, setPage] = useState<number>(1);
    const [tags, setTags] = useState<Tags>();

    useEffect(() => {
        populateWeatherData(page, pageSize);
    }, [page]);

    function GetTextFromBoolean(value: boolean): string {
        return value ? "Yes" : "No";
    }

    function GetCollectives(collectives: Collective[] | null): string {
        if (!collectives)
            return "";

        const names = collectives.map(collectivity => collectivity.name);
        return names.join('; ').toString();
    }

    function GetTextPopularity(count: number, counts: number[]): string {
        return MathHelper.calculatePopularity(count, counts).toFixed(2);
    }

    function handleSelected(selectedPage: number) {
        populateWeatherData(selectedPage, pageSize);
        setPage(selectedPage);
    }

    async function populateWeatherData(page: number, pageSize: number) {
        const response = await HttpClient.post<IPagination, Tags>('tags/getTagsFromStackOverflow', { page: page, pageSize: pageSize });
        setTags(Convert.toTags(JSON.stringify(response)))
    }

    const table = tags === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
        : <Table striped bordered>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Collectives</th>
                    <th>Has synonyms</th>
                    <th>Count</th>
                    <th>Popularity %</th>
                </tr>
            </thead>
            <tbody>
                {tags.items.map(item =>
                    <tr key={item.name}>
                        <td>{item.name}</td>
                        <td>{GetCollectives(item.collectives)}</td>
                        <td>{GetTextFromBoolean(item.hasSynonyms)}</td>
                        <td>{item.count}</td>
                        <td>{GetTextPopularity(item.count, tags.items.map(a => a.count))}</td>
                    </tr>
                )}
            </tbody>
        </Table>;

    const pagination = tags === undefined
        ? <></>
        : <PaginationComponent
            totalItems={40}
            pageSize={1}
            onSelect={(option) => handleSelected(option)}
            maxPaginationNumbers={5}
            defaultActivePage={1}
        />

    return (
        <Container className="mt-5">
            <h1 id="tabelLabel">Tags forecast</h1>
            <p>This component demonstrates fetching data from the Stack Overflow.</p>
            {table}
            <div className={"customPagination"}>
                {pagination}
            </div>
        </Container>
    );
}