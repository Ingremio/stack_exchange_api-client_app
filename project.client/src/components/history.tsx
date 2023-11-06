import { Location, NavigateFunction } from "react-router-dom";

type History = {
    navigate: NavigateFunction | null;
    location: Location | null;
}

const history: History = {
    navigate: null,
    location: null
}

export default history;